import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControl, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { NotifyService } from '../../../services/notify.service';

@Component({
  selector: 'app-contact-us',
  standalone: true,
  imports: [MatFormFieldModule, MatInputModule, MatCheckboxModule, MatButtonModule, CommonModule,ReactiveFormsModule],
  templateUrl: './contact-us.component.html',
  styleUrls: ['./contact-us.component.css']
})
export class ContactUsComponent implements OnInit {
  public contactForm: FormGroup;

  constructor(private formBuilder: FormBuilder, private notifyService: NotifyService) { }

  ngOnInit(): void {
    this.contactForm = this.formBuilder.group({
      nameControl: new FormControl('', [
        Validators.required,
        Validators.minLength(2),
        Validators.maxLength(50)
      ]),
      emailControl: new FormControl('', [
        Validators.required,
        Validators.minLength(2),
        Validators.maxLength(100),
        Validators.pattern('^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$')
      ]),
      messageControl: new FormControl('', [
        Validators.required,
        Validators.minLength(10),
        Validators.maxLength(1000)
      ])
    });
  }

  public canceled = false;

  public dialogCanceled() {
    this.canceled = true;
  }
}
