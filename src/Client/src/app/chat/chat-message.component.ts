import { Component, Input } from '@angular/core';
import { IUser } from '../shared/interfaces';
import { ColorFilter, FormatName } from '../utilities/filters';


@Component({
  selector: 'chat-message',
  templateUrl: './chat-message.component.html',
  styleUrls: ['./chat-message.component.css']
})

export class ChatMessageComponent {
  @Input() user: IUser;
  @Input() message: string;
  style: any;

  ngOnInit() {
    const color1 = ColorFilter(this.user.color1);
    const color2 = ColorFilter(this.user.color2);
    this.style = {
      'width': 100000000,
      'background-color': color1,
      'color': color2
    };
  }

  public formatName(str: string) {
    return FormatName(str);
  }

}
