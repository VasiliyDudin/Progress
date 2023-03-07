"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
var myName = document.getElementById("userInput").value; 

//Disable the send button until connection is established.
document.getElementById("sendButton").disabled = true;

//обрабатываем нажатие на кнопку
connection.on("ReceiveMessage", function (user, message) {
    //Получаем текущее дату и время
    var Data = new Date();
    var Year = Data.getFullYear();
    var Month = Data.getMonth();
    var Day = Data.getDate();
    var Hour = Data.getHours();
    var Minutes = Data.getMinutes();
    var Seconds = Data.getSeconds();
    //// Преобразуем месяца
    switch (Month) {
        case 0: var fMonth = "января"; break;
        case 1: var fMonth = "февраля"; break;
        case 2: var fMonth = "марта"; break;
        case 3: var fMonth = "апреля"; break;
        case 4: var fMonth = "мае"; break;
        case 5: var fMonth = "июня"; break;
        case 6: var fMonth = "июля"; break;
        case 7: var fMonth = "августа"; break;
        case 8: var fMonth = "сентября"; break;
        case 9: var fMonth = "октября"; break;
        case 10: var fMonth = "ноября"; break;
        case 11: var fMonth = "декабря"; break;
    }

    if (user == myName) {
        var div1 = document.createElement("div");
        div1.className = "d-flex justify-content-between";
        var div2 = document.createElement("div");
        div2.className = "d-flex flex-row justify-content-end mb-4 pt-1";
        var divInDiv2 = document.createElement("div");
        var p3 = document.createElement("p");
        var p1 = document.createElement("p");
        var p2 = document.createElement("p");
        var img = document.createElement("img");
        img.src = "/image/avatar/boy.png";
        img.style = "width: 45px; height: 100%;";
        img.alt = "imag1";
        p1.className = "small mb-1 text-muted";
        p2.className = "small mb-1";
        p3.className = "small p-2 me-3 mb-3 text-white rounded-3 bg-warning";
        // Вывод
        p1.innerText = Day + " " + fMonth + " " + Year + " года " + Hour + ":" + Minutes + ":" + Seconds;
        p2.innerText = user;
        p3.innerText = message;
        document.getElementById("messagesList").appendChild(div1);
        document.getElementById("messagesList").appendChild(div2);
        div1.appendChild(p1);
        div1.appendChild(p2);
        div2.appendChild(divInDiv2);
        divInDiv2.appendChild(p3);
        div2.appendChild(img);
    }
    else
    {
        var div1 = document.createElement("div");
        div1.className = "d-flex justify-content-between";
        var div2 = document.createElement("div");
        div2.className = "d-flex flex-row justify-content-start";
        var divInDiv2 = document.createElement("div");
        var p3 = document.createElement("p");
        var p1 = document.createElement("p");
        var p2 = document.createElement("p");
        var img = document.createElement("img");
        img.src = "/image/avatar/man-1.png";
        img.style = "width: 45px; height: 100%;";
        img.alt = "imag2";
        p1.className = "small mb-1 text-muted";
        p2.className = "small mb-1";
        p3.className = "small p-2 ms-3 mb-3 rounded-3";
        p3.style = "background-color: #f5f6f7;";
        // Вывод
        p1.innerText = Day + " " + fMonth + " " + Year + " года " + Hour + ":" + Minutes + ":" + Seconds;
        p2.innerText = user;
        p3.innerText = message;
        document.getElementById("messagesList").appendChild(div1);
        document.getElementById("messagesList").appendChild(div2);
        div1.appendChild(p2);
        div1.appendChild(p1);
        div2.appendChild(img);
        div2.appendChild(divInDiv2);
        divInDiv2.appendChild(p3);
       
    }
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});