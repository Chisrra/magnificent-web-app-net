import { NgFor } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, NgFor],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  title = 'Date me';
  http = inject(HttpClient);
  users: any;

  ngOnInit(): void {
    this.http.get("http://localhost:5189/api/v1/users/").subscribe({
      next: (response) => { this.users = response; },
      error: (error) => { console.error(error); },
      complete: () => { console.log("Request Completed"); }
    });
  }
}
