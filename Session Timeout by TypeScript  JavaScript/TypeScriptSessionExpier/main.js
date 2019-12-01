// If need to instal jQuery into project ... run "npm" in same folder as project root folder.
// npm install @types/jquery --save-dev
// npm install jquery --save 
//https://dev.vast.com/jquery-popup-overlay/
//https://tympanus.net/codrops/2013/11/07/css-overlay-techniques/
var ST;
(function (ST) {
    var SessionTimeout = /** @class */ (function () {
        function SessionTimeout(expireUrl, logoutUrl, extendUrl, timeoutInSecond, warningInSeconds) {
            var _this = this;
            this._expireUrl = expireUrl;
            this._logoutUrl = logoutUrl;
            this._extendUrl = extendUrl;
            this._timeoutInSecond = timeoutInSecond;
            this._warningInSeconds = warningInSeconds;
            this._blnCountDownt = false;
            this._expiration = Date.now() + this._timeoutInSecond * 1000; // millisecond
            $(document).keypress(function () { return _this.active(); }); // set event for key press
            $(document).click(function () { return _this.active(); }); //           for click    
            $('#continue').on('click', function () { return _this["continue"](); }); //find button and set event for "click"
            $('#logout').on('click', function () {
                _this._secondsRemaining = _this._timeoutInSecond;
                $('.popup-overlay, .main-content').removeClass('active');
                window.location.assign(_this._logoutUrl);
                return false; // must use - return false
            });
            $('#session-timeout-minute').html((this._timeoutInSecond / 60).toFixed(0)); //find button and set text timeout...
            $('#session-timeout-current-inactivity').html("0");
        }
        SessionTimeout.prototype.active = function () {
            // Expiration Time should changed in one place only.
            if (!this._blnCountDownt) {
                this._expiration = Date.now() + this._timeoutInSecond * 1000; // millisecond
            }
        };
        SessionTimeout.prototype.check = function () {
            var _this = this;
            var millisecondsRemaining = this._expiration - Date.now();
            this._secondsRemaining = Math.floor(millisecondsRemaining / 1000);
            if (this._secondsRemaining <= this._warningInSeconds) {
                $('.popup-overlay, .main-content').addClass('active');
                this._blnCountDownt = true; // if countdown do not allow change expiration...
                this.countdown();
            }
            else {
                setTimeout(function () { return _this.extend(); }, 1000);
                this._blnCountDownt = false;
                var delay = millisecondsRemaining - this._warningInSeconds * 1000;
                setTimeout(function () { return _this.check(); }, delay);
            }
            return false;
        };
        SessionTimeout.prototype.countdown = function () {
            var _this = this;
            $('#session-timeout-countdown').html(this._secondsRemaining.toString());
            var inactivity = (this._timeoutInSecond * 1000 - (this._expiration - Date.now())) / 1000 / 60;
            $('#session-timeout-current-inactivity').html(inactivity.toFixed(1));
            if (this._secondsRemaining <= 0) {
                $('.popup-overlay, .main-content').removeClass('active');
                window.location.assign(this._expireUrl);
            }
            else if (this._secondsRemaining > this._warningInSeconds) { }
            else {
                this._secondsRemaining -= 1;
                setTimeout(function () { return _this.countdown(); }, 1000);
            }
            ;
            return false;
        };
        SessionTimeout.prototype["continue"] = function () {
            var _this = this;
            this._blnCountDownt = false;
            this._expiration = Date.now() + this._timeoutInSecond * 1000; // millisecond
            var millisecondsRemaining = this._expiration - Date.now();
            this._secondsRemaining = Math.floor(millisecondsRemaining / 1000);
            $('.popup-overlay, .main-content').removeClass('active');
            var delay = (this._timeoutInSecond - this._warningInSeconds) * 1000;
            setTimeout(function () { return _this.extend(); }, 1000);
            setTimeout(function () { return _this.check(); }, delay);
            return false;
        };
        // Ask server - it erase server timeout value.
        // https://api.jquery.com/jquery.get/ 
        SessionTimeout.prototype.extend = function () {
            $.get(this._extendUrl, function (data) {
                $('#session-server-response').html(data);
            });
            return false;
        };
        return SessionTimeout;
    }());
    ST.SessionTimeout = SessionTimeout;
})(ST || (ST = {}));
/// <reference path="TypeScript/SessionTimeout.ts"/>
var timeoutInSeconds = 60;
var warningInSeconds = 40;
var expireUrl = "SessionExpired.aspx";
var logoutUrl = "LogOut.aspx";
var extendUrl = "Heartbeat.aspx"; //MUST USE runat="server" - for clear the TimeOut on the Server Side need to use aspx page.
var st = new ST.SessionTimeout(expireUrl, logoutUrl, extendUrl, timeoutInSeconds, warningInSeconds);
st.check();
//# sourceMappingURL=main.js.map