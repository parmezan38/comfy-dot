import { Component } from '@angular/core';
import { UserManager } from 'oidc-client';

import { AuthService } from '../services/auth.service';

@Component({
  selector: 'silent',
  templateUrl: './silent.component.html'
})

export class SilentComponent {
  constructor(private auth: AuthService) { }  
  ngOnInit() {
    new UserManager({ response_mode: "query" })
      .signinSilentCallback()
      .then(user => {
        if (!user) {
          this.auth.logout();
        }
      })
      .catch(e => {
        console.error(e);
      });
  }
}
