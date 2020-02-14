import { Component } from '@angular/core';
import { FormatName } from '../utilities/filters';
import { AuthService } from '../shared/auth.service';

@Component({
  selector: 'navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})

export class NavbarComponent {
  public userName: string;

  constructor(private authService: AuthService) { }

  ngOnInit() {
    this.authService.userChangeEvent
      .subscribe(user => {
        this.userName = FormatName(user.profile.name);
      });
  }

  public logout() {
    this.authService.logout();
  }
}
