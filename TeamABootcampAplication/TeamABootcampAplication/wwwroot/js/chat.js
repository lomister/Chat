"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chathub").build();
var Count;
var tempMessage;
var PrivateChatsUsername = new Array();
var PrivateChatsReceiverID = new Array();
var PrivateChatsID = new Array();

var isGroup = false;
var groupID = 0;
var groupName;
var beforeGroupName;

var privateChatID;

var _Username = document.cookie.match(new RegExp('(^| )' + 'username' + '=([^;]+)'));
if (match) _Username = match[2];

document.getElementById("sendButton").disabled = true;

connection.on("RecieveOpenPrivateChats", function (GroupChatName) { console.log(GroupChatName); });

connection.on("RecieveOpenGroupChats", function (GroupChatName, ID) { console.log(GroupChatName + " " + ID); });

connection.on("Private-Chat-User-List", function (OnlineUsers) {
    document.getElementById("modal-online-users").innerHTML = " ";
    var i = 0;
    for (i = 0; i < OnlineUsers.length; i++) {
        var MainTab = document.getElementById("modal-online-users");
        var NewDiv = document.createElement("div");

        var user = document.createElement("button");
        user.className = "create-chat-user btn btn-info btn-sm btn-marge";
        user.innerHTML = OnlineUsers[i];
        user.id = OnlineUsers[i];
        user.setAttribute("data-dismiss", "modal");
        user.addEventListener("click", function (event) {
            var id = event.target.id;
            connection.invoke("CreateNewChat", id, _Username);
            RefreshChatList();
        });
        NewDiv.appendChild(user);
        MainTab.appendChild(NewDiv);
    }
    //console.log(OnlineUsers);
});

function RefreshChatList() {
    document.getElementById("PrivateChatsList").innerHTML = " ";
    connection.invoke("GetYourPrivateChats", _Username);
    connection.invoke("GetAllGroupChats");
}

connection.on("GetOnline", function (OnlineUsers) {
    document.getElementById("onlineUsers").innerHTML = "";

    console.log(OnlineUsers);
    OnlineUsers.map(function (user) {
        var MainTab = document.getElementById("onlineUsers");
        var NewDiv = document.createElement("div");

        NewDiv.innerHTML = user;

        MainTab.appendChild(NewDiv);

    });
    //console.log(OnlineUsers);
});

connection.on("ReceiveMessage", function (user, message, date, server) {

    console.log("Receive messages");

    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var MainDiv = document.createElement("div");
    if (server == true) {
        MainDiv.className = "message-row other-message";
    }
    else {
        MainDiv.className = "message-row you-message";
    }

    var userName = document.createElement("div");
    userName.className = "message-user";
    userName.innerHTML = user;
    MainDiv.appendChild(userName);

    var Message = document.createElement("div");
    Message.className = "message-text";
    Message.innerHTML = msg;
    MainDiv.appendChild(Message);

    var Message = document.createElement("div");
    Message.className = "message-date";
    Message.innerHTML = date;
    MainDiv.appendChild(Message);

    document.getElementById("msg").appendChild(MainDiv);

    var elem = document.getElementById("msg");
    elem.scrollTop = elem.scrollHeight;
});

connection.on("ReceivePrivateMessage", function (user, message, date, server) {

    document.getElementById("MessageContainer").InnerHTML = "";

    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var MainDiv = document.createElement("div");
    if (server == true) {
        MainDiv.className = "message-row other-message";
    }
    else {
        MainDiv.className = "message-row you-message";
    }

    var userName = document.createElement("div");
    userName.className = "message-user";
    userName.innerHTML = user;
    MainDiv.appendChild(userName);

    var Message = document.createElement("div");
    Message.className = "message-text";
    Message.innerHTML = msg;
    MainDiv.appendChild(Message);

    var Message = document.createElement("div");
    Message.className = "message-date";
    Message.innerHTML = date;
    MainDiv.appendChild(Message);

    document.getElementById("MessageContainer").appendChild(MainDiv);

    var elem = document.getElementById("MessageContainer");
    elem.scrollTop = elem.scrollHeight;

    console.log(user + " " + message + " " + date);

});


connection.on("RecieveGroupChatMessage", function (message, username, date, server) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var MainDiv = document.createElement("div");
    if (server == true) {
        MainDiv.className = "message-row other-message";
    }
    else {
        MainDiv.className = "message-row you-message";
    }

    var userName = document.createElement("div");
    userName.className = "message-user";
    userName.innerHTML = username;
    MainDiv.appendChild(userName);

    var Message = document.createElement("div");
    Message.className = "message-text";
    Message.innerHTML = msg;
    MainDiv.appendChild(Message);

    var Message = document.createElement("div");
    Message.className = "message-date";
    Message.innerHTML = date;
    MainDiv.appendChild(Message);

    document.getElementById("msg").appendChild(MainDiv);

    var elem = document.getElementById("msg");
    elem.scrollTop = elem.scrollHeight;
});

connection.on("RecievePrivateChats", function (ReceiverUsername, userseed) {
    var MainDiv = document.createElement("div");
    MainDiv.className = "chat-guys";
    MainDiv.id = userseed; //get private chat seed
    MainDiv.innerHTML = ReceiverUsername;

    MainDiv.addEventListener("click", function (event) {
        var toUser = ReceiverUsername;
        privateChatID = event.target.id;
        document.getElementById("msg").innerHTML = " ";
        isGroup = false;
        connection.invoke("RecievePrivateChats1vs1", _Username, toUser, privateChatID);
    });
    document.getElementById("PrivateChatsList").appendChild(MainDiv);

    var elem = document.getElementById("PrivateChatsList");
    elem.scrollTop = elem.scrollHeight;
});

connection.on("ReceiveGroups", function (GroupName, GroupID) {
    var MainDiv = document.createElement("div");
    MainDiv.className = "chat-guys text-truncate";
    MainDiv.id = "group";
    MainDiv.innerHTML = GroupName;
    MainDiv.style = "background-color:red !important";

    MainDiv.addEventListener("click", function (event) {
        var name = GroupName;
        var id = GroupID;
        document.getElementById("msg").innerHTML = " ";
        isGroup = true;
        groupID = GroupID;

        connection.invoke("GetGroupChatMessage", GroupName, GroupID, _Username);
        groupName = GroupName;

    });
    document.getElementById("PrivateChatsList").appendChild(MainDiv);

    console.log(GroupID + ":" + GroupName);

    var elem = document.getElementById("PrivateChatsList");
    elem.scrollTop = elem.scrollHeight;

    console.log(PrivateChatID + " " + ReceiverUsername + " " + ReceiverID);
    });

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;

    connection.invoke("StoreUsername", _Username);
    connection.invoke("GetPrivateChatUsers", _Username);
    connection.invoke("GetYourPrivateChats", _Username);
    connection.invoke("GetAllGroupChats");
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("openUserList").addEventListener("click", function (event) {
    var user;

    var match = document.cookie.match(new RegExp('(^| )' + 'username' + '=([^;]+)'));
    if (match) user = match[2];
    connection.invoke("OpenUserList", user);
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user;

    var match = document.cookie.match(new RegExp('(^| )' + 'username' + '=([^;]+)'));
    if (match) user = match[2];
    var message = document.getElementById("messageInput").value;
    if (!isGroup) {
        if (message != "") {
            console.log("IMPORTANT for private: " + privateChatID);
            connection.invoke("SendMessage", user, 0, message, privateChatID).catch(function (err) {
                return console.error(err.toString());
            });
        }
    } else {
        if (message != "") {
            console.log("IMPORTANT for froup: " + groupName);
            connection.invoke("SendMessageToGroup", user, message, groupName, groupID).catch(function (err) {
                return console.error(err.toString());
            });
        }
    }
    document.getElementById("messageInput").value = "";
    event.preventDefault();



    //getMessages();
});

document.getElementById("creategroupchat-button").addEventListener("click", function (event) {
    var GroupName = document.getElementById("groupname").value;
    if (GroupName != "") {
        connection.invoke("CreateGroupChat", GroupName);
        RefreshChatList();
    }
});

document.getElementById("openUserList").addEventListener("click", function (event) { AddEventsToUsersList(); });
