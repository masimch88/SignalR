import { Component, OnInit } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { SignalRService } from './signal-r.service';
@Component({
	selector: 'app-root',
	templateUrl: './app.component.html',
	styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
	public MessageRecieved: boolean = false;
	public Messages: string[] = [];

	ngOnInit(): void {
		const connection = new signalR.HubConnectionBuilder()
			.configureLogging(signalR.LogLevel.Information)
			.withUrl('https://localhost:44349/chatHub', {
				transport: signalR.HttpTransportType.WebSockets,
				skipNegotiation: true
			})
			.build();

		connection.start().then(function () {
			console.log('SignalR Connected!');
		}).catch(function (err) {
			return console.error(err.toString());
		});

		connection.on("ReceiveMessage", (serverMessage: string) => {  
			this.MessageRecieved = true;
			this.Messages.push(serverMessage); 
			console.log('New message received from Server: ' + serverMessage);  
		}); 
	}

}
