ALTER TABLE interests ALTER COLUMN interest_id RESTART WITH 1;

insert into interests(interest_name) VALUES ('Sports'), ('Photography'), ('Travel'), ('Cooking'), ('Movies');