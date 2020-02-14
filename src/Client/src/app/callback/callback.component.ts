import { Component } from '@angular/core';
import { UserManager } from 'oidc-client';

@Component({
    selector: 'callback',
    templateUrl: './callback.component.html',
    styleUrls: ['./callback.component.css']
})

export class CallbackComponent {
    ngOnInit() {
        new UserManager({ response_mode: "query" })
            .signinRedirectCallback().then(() => {
                window.location.href = "/";
            })
            .catch(e => {
                console.error(e);
            });
    }
}
