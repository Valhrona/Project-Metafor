const CSV = require('fast-csv');
const fs = require('fs');
const slugify = require('slugify');

//output file

var outputFile = fs.openSync('../../data/rdf/BIM.ttl', 'w');

const source_names = ['BIM_terms.csv'];

const PREFIXES = `
@prefix rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#> .
@prefix rdfs: <http://www.w3.org/2000/01/rdf-schema#> .
@prefix owl: <http://www.w3.org/2002/07/owl#> .
@prefix xsd: <http://www.w3.org/2001/XMLSchema#> .
@prefix dl: <urn:deloitte:> .
@prefix ex: <urn:ex:> .
`;
fs.writeSync(outputFile, PREFIXES);
//-------------------------------------------------------------------------------

//-----------start processing
source_names.forEach((source_name, index) => {
    let i = 0
    fs.createReadStream('../../data/csv/' + source_name)
        .pipe(CSV.parse({ headers: true, delimiter: ';' }))
        .on('data', item => {
            i++;
            //console.log(item)
            if (source_name === 'BIM_terms.csv') { 
                fs.writeSync(outputFile,`
                ex:t_${item['Term'].replace(/\s/g, '_').replace(/[^\w\s]/gi, '_')} a dl:Data_Requirement, dl:BIM_Concept ;
                dl:definition """${encodeURIComponent(item['Definition'])}""";
                rdfs:label "${item['Term']}" .
                `);
                if (item['Subject area']) {
                    fs.writeSync(outputFile,`
                    ex:e_${item['Subject area'].replace(/\s/g, '_').replace(/[^\w\s]/gi, '_')} a dl:Subject_Area ; rdfs:label """${item['Subject area']}""" .
                    ex:t_${item['Term'].replace(/\s/g, '_').replace(/[^\w\s]/gi, '_')} dl:subject_area ex:e_${item['Subject area'].replace(/\s/g, '_').replace(/[^\w\s]/gi, '_')} .
                    `);
                }
            }
        });
});



