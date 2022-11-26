import { HttpClient } from '@angular/common/http';
import { Component,OnInit } from '@angular/core';



@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements  OnInit {
  
  title="Dating App";
  users:any;
constructor(private client:HttpClient){}

  ngOnInit(): void {
    this.client.get('https://localhost:5001/api/users').subscribe({
      next: response => this.users = response,
      error: error => console.log(error),
      complete: () => console.log("Request has completed")
    });
  }




}