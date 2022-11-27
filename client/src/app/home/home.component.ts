import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  registerMode = false;
  users:any;
  constructor(private client:HttpClient) { }

  ngOnInit(): void {
    this.getUsers();
  }

  getUsers(){
    this.client.get('https://localhost:5001/api/users').subscribe({
      next: response => this.users = response,
      error: error => console.log(error),
      complete: () => console.log("Request has completed")
    });
  }

  registerToggle(){
    this.registerMode = !this.registerMode;
  }

  cancelRegisterMode(event:boolean){
    this.registerMode = event
  }
}
