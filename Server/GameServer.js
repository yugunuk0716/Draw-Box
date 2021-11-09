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
            //console.log(req.loginUser);
            //console.log(true);
        }
        else {
            res.json({result:false, payload:"변형된 토큰이 감지됨"});
            console.log("token === undefined");
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

app.post("/scorerecord", async (req,res) => {
    if(req.loginUser != null) {
        let {score} = req.body;
        //let name = "";

        let userId = req.loginUser.id;

        let sql = `SELECT * FROM box_high_scores WHERE box_user = ?`;
        let [result] = await pool.query(sql,[userId]);
        
        if(result.length > 0) {
            if(result[0].score < score) {
                sql = `UPDATE box_high_scores SET score = ?, WHERE user = ?`;
                await pool.query(sql,[score,userId]);
                res.json({result:true,payload:"성공적으로 업데이트 되었습니다"});
            }
            else {
                res.json({result:false,payload:"기록이 낮아 갱신하지 않습니다"});
            }
        }
        else {
            sql = `SELECT * FROM box_user WHERE id = ?`;
            let [r] = await pool.query(sql,[userId]);
            let name = r[0].name;

            let re = await insertData(name,score,userId);
            if(re) {
                res.json({result:true, payload:"기록 완료"});
            }
            else {
                res.json({result:false,payload:"기록 실패"});
            }
        }
    }
});
app.get("/ranklist", async (req,res) => {
    if(req.loginUser != null) {
        let sql = `SELECT * FROM box_high_scores ORDER BY score DESC LIMIT 0,10`;
        let [list] = await pool.query(sql);

        res.json({result:true,payload:JSON.stringify({list})});
    }
    else {
        res.json({result:false,payload:"로그인 시 이용할 수 있습니다."})
    }
});
app.get("/rankconfirm", async (req,res) => {
    if(req.loginUser != null) {
        let userId = req.loginUser.id
        let sql = `SELECT * FROM box_high_scores WHERE box_user = ?`;
        let [list] = await pool.query(sql,[userId]);

        if(list.length > 0) {
            res.json({result:true,payload:"처음 플레이하는 유저가 아닙니다."});
        }
        else {
            res.json({result:false,payload:"처음 플레이하는 유저입니다."});
        }
    }
});
    
server.listen(54000, () => {
    console.log("서버가 54000번 포트에서 구동중입니다");
});