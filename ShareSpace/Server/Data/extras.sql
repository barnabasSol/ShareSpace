-- \i /Users/'Barnabas Solomon'/OneDrive/Desktop/web/'ASPNET projects'/ShareSpaceSolution/ShareSpace/Server/Data/extras.sql

ALTER TABLE interests ALTER COLUMN interest_id RESTART WITH 1;

insert into interests(interest_name) VALUES ('Sports'), ('Photography'), ('Travel'), ('Cooking'), ('Movies');

-- create extension if not exists "uuid-ossp";