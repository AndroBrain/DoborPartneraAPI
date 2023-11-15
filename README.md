# DatingApplication

### Jak używać endpointu `/matches`

#### Parametry
+ `user_id` - definiuje, dla kogo mają być wyszukane potencjalne pary
+ `num_matches` - definiuje ile najlepszych par ma być zwróconych spośród wszystkich

### Wartość zwracana

Endpoint zwraca listę JSON'ów, z czego każdy zawiera trzy pola:
+ `score` - compatybilność z zakresu <0, 1>
+ `target_user_id` - id osoby, z którą parowany był użytkownik
+ `user_id` - id przekazane w parametrach wejściowych

### Przykład

`localhost:5000/matches?user_id=1&num_matches=3`

daje:

`[{"score":0.9227024042076089,"target_user_id":6,"user_id":1},{"score":0.9204788825745897,"target_user_id":4,"user_id":1},{"score":0.8993866036393809,"target_user_id":3,"user_id":1}]`

### Jak używać endpointu `/generate_users`

#### Parametry
+ `num_users` - definiuje liczbę nowych użytkowników do wygenerowania, domyślnie 1

### Wartość zwracana

Lista obiektów JSON reporezentujących podstawowe dane nowo utworzonych użytkowników.

### Przykład

`localhost:5000/generate_users?num_users=3`

daje:

    [{
        "date_of_birth": "Wed, 03 Nov 1982 00:00:00 GMT",
        "description": "Better rate yourself before medical else natural.",
        "email": "vmedina@example.net",
        "first_name": "Francisco",
        "gender": "male",
        "second_name": "Moran",
        "sexuality": "homosexual",
        "user_id": 1
    },
    {
        "date_of_birth": "Mon, 12 May 1986 00:00:00 GMT",
        "description": "Sister fall glass ten.",
        "email": "pgeorge@example.org",
        "first_name": "Carrie",
        "gender": "female",
        "second_name": "Mcclure",
        "sexuality": "heterosexual",
        "user_id": 2
    },
    {
        "date_of_birth": "Tue, 11 Jan 1972 00:00:00 GMT",
        "description": "Realize money visit control leave lead.",
        "email": "pattersonjoseph@example.net",
        "first_name": "Sabrina",
        "gender": "female",
        "second_name": "Choi",
        "sexuality": "heterosexual",
        "user_id": 3
    }]