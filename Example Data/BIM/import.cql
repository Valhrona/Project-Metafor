
MATCH (n) DETACH DELETE n ;
//DROP DATABASE neo4j; CREATE DATABASE neo4j;
CALL n10s.graphconfig.init({
	handleMultival: 'ARRAY',
	multivalPropList: []
});
CALL n10s.nsprefixes.add('ex', 'urn:ex:');
CALL n10s.nsprefixes.add('dl', 'urn:deloitte:');

DROP CONSTRAINT n10s_unique_uri IF EXISTS;
CREATE CONSTRAINT n10s_unique_uri ON (r:Resource) ASSERT r.uri IS UNIQUE;

// RDF import
CALL n10s.rdf.import.fetch("file:////C:/Users/aariantari/Downloads/rdf/BIM.ttl","Turtle-star");
CALL n10s.rdf.import.fetch("file:////C:/Users/aariantari/Downloads/rdf/Reporting.ttl","Turtle-star");

//post-processing


