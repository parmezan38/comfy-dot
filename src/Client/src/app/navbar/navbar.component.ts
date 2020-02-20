import { Component } from '@angular/core';
import { FormatName } from '../shared/filters';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})

export class NavbarComponent {
  public userName: string;

  constructor(private auth: AuthService) { }

  ngOnInit() {
    this.auth.userChangeEvent
      .subscribe(user => {
        this.userName = FormatName(user.profile.name);
      });
  }

  public logout() {
    this.auth.logout();
  }
}
