import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

import { IResponseMessage } from '../shared/interfaces';

@Injectable()
export class SnackbarService {
  private subject = new Subject<any>();
  observable = this.subject.asObservable();

  public show(snackbarMessage: IResponseMessage) {
    this.subject.next(snackbarMessage);
  }
}
