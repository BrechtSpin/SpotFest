import { Injectable, inject } from '@angular/core';
import { HubConnection, HubConnectionBuilder, HubConnectionState, LogLevel } from '@microsoft/signalr';

import { environment } from '@env/environment';

@Injectable({ providedIn: 'root' })
export class happeningHub {
  private hub!: HubConnection;
  private readonly hubUrl = `${environment.apiHappeningUrl}/currenthub`;

  async startConnection(): Promise<void> {
    if (this.hub
      && this.hub.state !== HubConnectionState.Disconnected
      && this.hub.state !== HubConnectionState.Connecting) {
      return;
    }

    this.hub = new HubConnectionBuilder()
      .withUrl(`${this.hubUrl}`)
      .withAutomaticReconnect()
      .configureLogging(LogLevel.Information)
      .build();
    await this.hub.start();
  }

  async invoke<T>(methodName: string): Promise<T> {
    if (!this.hub) {
      throw new Error('Hub connection is not established.');
    }
    return await this.hub.invoke<T>(methodName);
  }

  on<T>(methodName: string, newMethod: (data: T) => void): void {
    if (!this.hub) {
      throw new Error('Hub connection is not established.');
    }
    this.hub.on(methodName, newMethod);
  }
}
