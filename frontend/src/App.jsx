import { useEffect, useState } from 'react'
import './App.css'

function App() {

  const API_URL = "http://localhost:5243/veiculos";

  const [veiculos, setVeiculos] = useState([]);
  const [motor, setMotor] = useState('');
  const [especificacaoMotor, setEspecificacaoMotor] = useState('');
  const [nome, setNome] = useState('');
  const [marca, setMarca] = useState('');

  const buscarVeiculos = async () => {
    try {
      const response = await fetch(API_URL);
      const data = await response.json;
      setVeiculos(data);
    } catch (erro) {
      console.error(erro);
    }
  };

  useEffect(() => {
    buscarVeiculos();
  }, []);

  return (
    <>
      <div>
        <p>Lista de Veículos</p>
        {veiculos.length === 0 ? (
          <p>Nenhum veículo cadastrado</p>
        ) : (
          <table>
            <thead>
              <th>
                <tr>Marca</tr>
                <tr>Nome</tr>
                <tr>Motor</tr>
                <tr>Especificação do motor</tr>
              </th>
            </thead>
            <tbody>
              {veiculos.map((veiculo, index) =>
                <tr key={index}>
                  <td>{veiculo.marca}</td>
                  <td>{veiculo.nome}</td>
                  <td>{veiculo.motor}</td>
                  <td>{veiculo.especificacaoMotor}</td>
                </tr>
              )}
            </tbody>
          </table>
        )
        }
      </div>
    </>
  )
}

export default App
