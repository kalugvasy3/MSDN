// If need to instal jQuery into project ... run "npm" in same folder as project root folder.
// npm install @types/jquery --save-dev
// npm install jquery --save 

//https://dev.vast.com/jquery-popup-overlay/
//https://tympanus.net/codrops/2013/11/07/css-overlay-techniques/

namespace ST {

    export class SessionTimeout {
        private _expireUrl: string;
        private _logoutUrl: string;
        private _extendUrl: string;
        private _timeoutInSecond: number;
        private _warningInSeconds: number;

        private _expiration: number;
        private _secondsRemaining: number;
        private _blnCountDownt: boolean;

        public constructor(expireUrl: string, logoutUrl: string, extendUrl: string, timeoutInSecond: number, warningInSeconds: number) {
            this._expireUrl = expireUrl;
            this._logoutUrl = logoutUrl;
            this._extendUrl = extendUrl;
            this._timeoutInSecond = timeoutInSecond;
            this._warningInSeconds = warningInSeconds;

            this._blnCountDownt = false;

            this._expiration = Date.now() + this._timeoutInSecond * 1000; // millisecond

            $(document).keypress(() => this.active());          // set event for key press
            $(document).click(() => this.active());             //           for click    

            $('#continue').on('click', () => this.continue());  //find button and set event for "click"
            $('#logout').on('click', () => {
                this._secondsRemaining = this._timeoutInSecond;
                $('.popup-overlay, .main-content').removeClass('active');
                window.location.assign(this._logoutUrl);
                return false;  // must use - return false
            });

            $('#session-timeout-minute').html((this._timeoutInSecond / 60).toFixed(0));  //find button and set text timeout...
            $('#session-timeout-current-inactivity').html("0");
        }

        private active(): void {
            // Expiration Time should changed in one place only.
            if (!this._blnCountDownt) {
                this._expiration = Date.now() + this._timeoutInSecond * 1000; // millisecond
            }
        }

        public check(): boolean {

            let millisecondsRemaining = this._expiration - Date.now();
            this._secondsRemaining = Math.floor(millisecondsRemaining / 1000);

            if (this._secondsRemaining <= this._warningInSeconds) {
                $('.popup-overlay, .main-content').addClass('active');
                this._blnCountDownt = true; // if countdown do not allow change expiration...
                this.countdown();
            } else {               
                setTimeout(() => this.extend(), 1000);
                this._blnCountDownt = false;
                let delay = millisecondsRemaining - this._warningInSeconds * 1000;
                setTimeout(() => this.check(), delay);
             }

            return false
        }

        private countdown(): boolean {

            $('#session-timeout-countdown').html(this._secondsRemaining.toString());

            let inactivity = (this._timeoutInSecond * 1000 - (this._expiration - Date.now())) / 1000 / 60;
            $('#session-timeout-current-inactivity').html(inactivity.toFixed(1));

            if (this._secondsRemaining <= 0) {
                $('.popup-overlay, .main-content').removeClass('active');
                window.location.assign(this._expireUrl);              
            }
            else if (this._secondsRemaining > this._warningInSeconds) { }
            else {
                this._secondsRemaining -= 1;
                setTimeout(() => this.countdown(), 1000);
            };
            return false;
        }

        private continue(): boolean {
            this._blnCountDownt = false;
            this._expiration = Date.now() + this._timeoutInSecond * 1000; // millisecond
            let millisecondsRemaining = this._expiration - Date.now();
            this._secondsRemaining = Math.floor(millisecondsRemaining / 1000);

            $('.popup-overlay, .main-content').removeClass('active');
            let delay = (this._timeoutInSecond - this._warningInSeconds) * 1000;
            setTimeout(() => this.extend(), 1000);          
            setTimeout(() => this.check(), delay);          
            return false;
        }


        // Ask server - it erase server timeout value.
        // https://api.jquery.com/jquery.get/ 

        private extend(): boolean {

            $.get(this._extendUrl,(data) => {
                $('#session-server-response').html(data);
            })
            return false;
        }
    }
}