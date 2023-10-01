const express = require('express');
const app = express();
const port = 3000;

app.Listen(port, () => { 
    console.log('sever is running on port ${port}');
})