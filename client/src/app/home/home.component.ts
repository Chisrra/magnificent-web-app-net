import { Component, inject, OnInit } from '@angular/core';
import { RegisterComponent } from "../register/register.component";
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [RegisterComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit {
  registerMode = false;
  http = inject(HttpClient);
  users: any;

  ngOnInit(): void {
    this.getUsers();
  }

  registerToggle(): void {
    this.registerMode = !this.registerMode;
  }

  cancelRegisterMode(event: boolean): void {
    this.registerMode = event;
  }

  getUsers() {
    this.http.get("http://localhost:5189/api/users/").subscribe({
      next: (response) => { this.users = response; },
      error: (error) => { console.error(error); },
      complete: () => { console.log("Request Completed"); }
    });
  }
}
