/// <reference path="TypeScript/SessionTimeout.ts"/>

let timeoutInSeconds = 60; 
let warningInSeconds = 40;
let expireUrl = "SessionExpired.aspx";
let logoutUrl = "LogOut.aspx";
let extendUrl = "Heartbeat.aspx"; //MUST USE runat="server" - for clear the TimeOut on the Server Side need to use aspx page.


let st = new ST.SessionTimeout(expireUrl, logoutUrl, extendUrl,timeoutInSeconds, warningInSeconds);
st.check();










