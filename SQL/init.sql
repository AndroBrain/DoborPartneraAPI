CREATE TABLE users
(
  id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
  email VARCHAR(100),
  password_hash BYTEA,
  password_salt BYTEA
);

CREATE TABLE users_info
(
  user_id INT PRIMARY KEY,
  name VARCHAR(50),
  surname VARCHAR(50),
  gender VARCHAR(10),
  birthdate DATE,
  description VARCHAR(1000),
  avatar VARCHAR(1000),
  CONSTRAINT fk_user_info
	FOREIGN KEY(user_id)
	  REFERENCES users(id)
);

CREATE TABLE user_images
(
  id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
  user_id INT,
  url VARCHAR(10000),
  CONSTRAINT fk_user
  FOREIGN KEY(user_id)
    REFERENCES users(id)
);

CREATE TABLE user_interests
(
  id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
  user_id INT,
  interest VARCHAR(100),
  CONSTRAINT fk_user
  FOREIGN KEY(user_id)
    REFERENCES users(id)
);

CREATE TABLE messages
(
  id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
  from_user INT,
  to_user INT,
  text VARCHAR(10000),
  timestamp BIGINT,
  CONSTRAINT fk_from_user
  FOREIGN KEY(from_user)
    REFERENCES users(id),
  CONSTRAINT fk_to_user
  FOREIGN KEY(to_user)
    REFERENCES users(id)
);

CREATE TABLE declined_matches
(
  id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
  user_id INT,
  declined_user_id INT,
  CONSTRAINT fk_user
  FOREIGN KEY(user_id)
    REFERENCES users(id),
  CONSTRAINT fk_declined_user
  FOREIGN KEY(declined_user_id)
    REFERENCES users(id)
);

CREATE TABLE tests
(
  id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
  user_id INT,
  eyes INT,
  hair INT,
  tattoo INT,
  sport INT,
  education INT,
  recreation INT,
  family INT,
  charity INT,
  people INT,
  wedding INT,
  belief INT,
  money INT,
  religious INT,
  mind INT,
  humour INT,
  FOREIGN KEY(user_id)
    REFERENCES users(id)
);
