import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { WorkerService } from '../../services/worker';
import { WorkerInfoDto } from '../../models/auth';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './home.html',
  styleUrls: ['./home.css']
})
export class HomeComponent implements OnInit{
  
  constructor(private router: Router,private workerService:WorkerService) { }
  currentWorker!:WorkerInfoDto;
  ngOnInit(): void {
    this.currentWorker=this.workerService.currentWorker;
    console.log(this.currentWorker);
    
  }

  // ניווט לדף חברות עובדת
  navigateToWorkerCompanies(): void {
    if(this.currentWorker!=null)
    this.router.navigate([`/workers/${this.currentWorker.id}/companies`]);
  }
   navigateToReports() {
    this.router.navigate(['/reports']);
  }
}