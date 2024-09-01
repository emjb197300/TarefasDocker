import { HttpClient, HttpClientModule } from '@angular/common/http';
import { Component, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Observable } from 'rxjs';
import { Tarefa } from '../models/tarefa.model';
import { AsyncPipe, DatePipe } from '@angular/common';
import { CommonModule } from '@angular/common';
import { EmailValidator, FormControl, FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
/* *********************************************** */
/* Importando a configuração de algumas linguagens */
import { registerLocaleData } from '@angular/common';
import localePT from '@angular/common/locales/pt';
registerLocaleData(localePT);

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, HttpClientModule, AsyncPipe, FormsModule, ReactiveFormsModule, CommonModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
  providers: [DatePipe]
})
export class AppComponent {
  http = inject(HttpClient);

  urlApi: string = 'http://localhost:8000/api/Tarefa';

  tarefasForm = new FormGroup({
    id: new FormControl<string | '0'>('0'),
    descricao: new FormControl<string>(''),
    data: new FormControl<string | null>(''),
    status: new FormControl<boolean>(false)
  })

  tarefas$ = this.getTarefas();

  showCriar: boolean = true;
  showEditar: boolean = false;
  showCancelar: boolean = false;
  
  constructor(private datePipe: DatePipe) {}


  onInit() {

    this.showCriar = true;
    this.showEditar = false;
    this.showCancelar = false;
    this.tarefas$ = this.getTarefas();
    this.tarefasForm.reset();
    this.tarefasForm.patchValue({
      id: "0",
      status: false
    });
    
  }

  onFormSubmit() {

   
    const addTarefaRequest = {
      id: this.tarefasForm.value.id,
      descricao: this.tarefasForm.value.descricao,
      data: this.tarefasForm.value.data,
      status: this.tarefasForm.value.status,
    }


    if(addTarefaRequest.id == "0")
    {
      this.http.post(this.urlApi, addTarefaRequest)
      .subscribe({
        next: (value) => {
          console.log(value);
          
          this.tarefas$ = this.getTarefas();
          this.tarefasForm.reset();
          this.onInit();
          alert('Tarefa criada com sucesso !');
        }
      });
    }
    else
    {

      this.http.put(this.urlApi + `/${addTarefaRequest.id}`, addTarefaRequest)
      .subscribe({
        next: (value) => {
          console.log(value);
          this.tarefas$ = this.getTarefas();
          this.tarefasForm.reset();
          this.onInit();
          alert('Tarefa alterada com sucesso !');
          
        }
      });      
    }

    

  }

  onDelete(id: string) {
    this.http.delete(this.urlApi + `/${id}`)
    .subscribe({
      next: (value) => {
        alert('Tarefa deletada');
        this.tarefas$ = this.getTarefas();
        this.tarefasForm.reset();
      }
    })
  }

  onEdit(id: string) {
    this.http.get(this.urlApi + `/${id}`)
    .subscribe({
      next: (data: any) => {
       
        this.tarefasForm.setValue({
          id: data.id,
          descricao: data.descricao,
          data: this.datePipe.transform(data.data, "yyyy-MM-dd"),
          status: data.status
        });

        this.showCriar = false;
        this.showEditar = true;
        this.showCancelar = true;

        
      }
    })
  }

private getTarefas(): Observable<Tarefa[]> {
  return this.http.get<Tarefa[]>(this.urlApi);
}
}


