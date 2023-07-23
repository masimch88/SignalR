import { Injectable } from '@angular/core';
import * as signalR from "@microsoft/signalr";

declare var $: any;
@Injectable({
	providedIn: 'root'
})
export class SignalRService {

	private hubConnection: signalR.HubConnection | undefined;

    public startConnection = () => {
      this.hubConnection = new signalR.HubConnectionBuilder()
                              .withUrl('http://localhost:44349/chatHub')
                              .build();
      this.hubConnection
        .start()
        .then(() => console.log('Connection started'))
        .catch(err => console.log('Error while starting connection: ' + err))
    }

}
