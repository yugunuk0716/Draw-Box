let secret = require('./secret');

const mysql = require('mysql2');

const pool = mysql.createPool(secret);
const promisePool = pool.promise(); //프로미스 기반의 풀을 만듦

async function insertData(name, score,id) {
    let sql = `INSERT INTO box_high_scores (name,score,box_user) VALUES (?,?,?)`;
    let result = await promisePool.query(sql,[name,score,id]);

    //console.log(result[0]);

    return result[0].affectedRows == 1;
}

module.exports = {
    pool:promisePool,
    insertData
};