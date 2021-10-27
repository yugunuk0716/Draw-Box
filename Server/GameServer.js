const express = require('express');
const http = require('http');
const path = require('path');
const fs = require('fs/promises');
const {pool,insertData} = require('./DB');
const jwt = require('jsonwebtoken');

const key = require('./secretKey');

const app = express();

app.use(express.json()); //이 녀석은 요청을 json으로 변환해주는 역할을 함.

app.use((req,res,next) => {
    let auth = req.headers["authorization"];
    if(auth === undefined) {
        req.loginUser = null;
        next();
        return;
    }
    auth = auth.split(" ");
    let token = auth[1];

    if(token !== undefined) {
        const decode = jwt.verify(token,key); //해당 토큰이 해당키로 암호화 되었는지 체크
        if(decode) {
            req.loginUser = decode;
        }
        else {
            res.json({result:false, payload:"변형된 토큰이 감지됨"});
            return;
        }
    }
    else {
        req.loginUser = null;
    }
    next();
});

//app이 요청이 왔을때 응답을 해주는 함수
const server = http.createServer(app);

app.post("/register", async (req,res) => {
    let {name,id,password} = req.body;

    let sql = `SELECT * FROM \`box_user\` WHERE id = ?`; //작성해서 입력한 id의 회원이 존재하는지 먼저 검사
    let [result] = await pool.query(sql,[id]);

    if(result.length > 0) {
        res.json({result:false,payload:"중복된 회원이 존재합니다"});
        return;
    }

    sql = `INSERT INTO box_user (id, name, password) VALUES (?,?,PASSWORD(?))`;

    await pool.query(sql,[id,name,password]);

    res.json({result:true, payload:"성공적으로 회원가입"});
});

app.post("/login", async (req,res) => {
    let {id,password} = req.body;
    let sql = `SELECT * FROM \`box_user\` WHERE id = ? AND password = PASSWORD(?)`; //작성해서 입력한 id의 회원이 존재하는지 먼저 검사
    const [row] = await pool.query(sql,[id,password]);

    if(row.length > 0) {
        let {code,id,name} = row[0];
        //회원이 존재한다면
        const token = jwt.sign({code,id,name},key, {
            expiresIn:'30 days'
        });
        //console.log(token);
        res.json({result:true,payload:token});
    }
    else {
        //회원이 존재하지 않는다면
        res.json({result:false, payload:"존재하지 않는 회원입니다"});
    }
});
    
server.listen(54000, () => {
    console.log("서버가 54000번 포트에서 구동중입니다");
});