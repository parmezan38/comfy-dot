import { Component } from '@angular/core';
import { SnackbarService } from '../services/snackbar.service';
import { IResponseMessage } from '../shared/interfaces';

@Component({
  selector: 'snackbar',
  templateUrl: './snackbar.component.html',
  styleUrls: ['./snackbar.component.css']
})
export class SnackbarComponent {
  type: string;
  message: string;
  isVisible: boolean;

  constructor(private snackbarService: SnackbarService) {
    this.snackbarService.observable.subscribe((res: IResponseMessage) => this.show(res));
  }

  show(res: IResponseMessage) {
    this.type = res.type;
    this.message = res.message;
    this.isVisible = true;
    setTimeout(() => {
      this.isVisible = false;
      this.type = '';
      this.message = '';
    }, 3000);
  }

}
