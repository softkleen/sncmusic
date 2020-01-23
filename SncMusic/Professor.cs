﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;
using MySql.Data;

namespace SncMusic
{
    public class Professor
    {
        private int id;
        // atributos e propriedades
        //public int Id { get; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
       
        public string Email { get; set; }
        public string Telefone { get; set; }
        public DateTime DataCadastro { get; set; }
        public int Id { get => id; set => id = value; }

        //métodos construtores
        public Professor()
        {

        }
        public Professor(int _id, string _nome, string _cpf,  string _email, string _telefone, DateTime _dataCadastro)
        {
            Id = _id;
            Nome = _nome;
            Cpf = _cpf;
            Telefone = _telefone;
            Email = _email;
            DataCadastro = _dataCadastro;
        }
        public Professor(string _nome, string _cpf, string _email, string _telefone)
        {
            Nome = _nome;
            Cpf = _cpf;
            Telefone = _telefone;
            Email = _email;
        }
        public Professor(int _id, string _nome, string _telefone)
        {
            Nome = _nome;
            Telefone = _telefone;
            Id = _id;
        }
        //métodos da classe
        public void Inserir()
        {
            MySqlCommand comm = Banco.Abrir();
            comm.CommandText = "insert into tb_professor values (0,@nome,@cpf,@email,@telefone,default)";
            comm.Parameters.Add("@nome", MySqlDbType.VarChar).Value = Nome;
            comm.Parameters.Add("@cpf", MySqlDbType.VarChar).Value = Cpf;
            comm.Parameters.Add("@email", MySqlDbType.VarChar).Value = Email;
            comm.Parameters.Add("@telefone", MySqlDbType.VarChar).Value = Telefone;
            comm.ExecuteNonQuery();
            comm.CommandText = "select @@identity";
            Id = Convert.ToInt32(comm.ExecuteScalar());
            comm.Connection.Close();
        }
        public bool Alterar(Professor professor)
        {
            try //bloco de tratamento de excessão
            {
                var comm = Banco.Abrir();
                comm.CommandText = "update tb_professor set nome_professor = @nome," +
                    "telefone_professor=@telefone where id_professor = @id";
                comm.Parameters.Add("@nome", MySqlDbType.VarChar).Value = professor.Nome;
                comm.Parameters.Add("@telefone", MySqlDbType.VarChar).Value = professor.Telefone;
                comm.Parameters.Add("@id", MySqlDbType.Int32).Value = professor.Id;
                comm.ExecuteNonQuery();
                Banco.Fechar();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
        public void ConsultarPorId(int _id)
        {
            //consulte o professor
            var comm = Banco.Abrir();
            comm.CommandText = "select * from tb_professor where id_professor = " + _id;
            var dr = comm.ExecuteReader();
            while (dr.Read())
            {
                Nome = dr.GetString(1);
                Email = dr.GetString(4);
                Cpf = dr.GetString(2);
                Telefone = dr.GetString(4);
                DataCadastro = Convert.ToDateTime(dr.GetValue(5));
            }
            Banco.Fechar();
        }
        public List<Professor> ListarTodos()
        {
            List<Professor> listaProfessor = new List<Professor>();
            var comm = Banco.Abrir();
            comm.CommandText = "select * from tb_professor order by 2 ";
            var dr = comm.ExecuteReader();
            while (dr.Read())
            {
                listaProfessor.Add(new Professor(dr.GetInt32(0),
                    dr.GetString(1),
                     dr.GetString(2), dr.GetString(3),
                     dr.GetString(4),
                     Convert.ToDateTime(dr.GetValue(5))));
            }
            Banco.Fechar();

            return listaProfessor;
        }

    }
}
