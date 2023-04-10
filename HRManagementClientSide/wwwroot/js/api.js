
fetch('https://localhost:7269/api/Employees')
    .then(response => response.json())
    .then(data => console.log(data))
    .catch(error => console.error(error));