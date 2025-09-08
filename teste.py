import requests

requests.post("https://localhost:5001/api/maquina/cadastro/", json = {
    "Nome": "m1",
    "Patrimonio": "p1234",
    "Id": 123,
    "Local": {
        "Campus": 2,
        "Predio": 12,
        "Andar": 4,
        "Numero": 405
    }
}, verify=False)