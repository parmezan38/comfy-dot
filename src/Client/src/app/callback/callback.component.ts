import { Router } from '@angular/router';
import { Component } from '@angular/core';
import { UserManager } from 'oidc-client';

@Component({
  selector: 'callback',
  templateUrl: './callback.component.html'
})

export class CallbackComponent {
  constructor(private router: Router) { }

  ngOnInit() {
    new UserManager({ response_mode: "query" })
      .signinRedirectCallback()
      .then(() => {
        this.router.navigate(['/']);
      })
      .catch(e => {
        console.error(e);
      });
  }
}
