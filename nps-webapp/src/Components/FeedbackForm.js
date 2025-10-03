// src/components/FeedbackForm.js
import React, { useState } from 'react';
import axios from 'axios'; // Importe o axios
import './FeedbackForm.css';

function FeedbackForm() {
  const [score, setScore] = useState(null);
  const [comment, setComment] = useState('');
  const [statusMessage, setStatusMessage] = useState(''); // Estado para mensagens de sucesso/erro

  //Transforma a função em async para usar await
  const handleSubmit = async (event) => {
    event.preventDefault();

    // Validação simples para garantir que uma nota foi selecionada
    if (score === null) {
      setStatusMessage('Por favor, selecione uma nota de 0 a 10.');
      return;
    }

    setStatusMessage('Enviando feedback...');

    // Montando o objeto de dados (payload)
    const feedbackData = {      
      userId: 'user-frontend-mvp', 
      score: score,
      comment: comment,
    };

    try {
      //  Faz a chamada POST para a API back-end
      const response = await axios.post('https://localhost:7271/api/FeedbackNps', feedbackData);

      // Lidar com a resposta de sucesso
      if (response.status === 201) { // 201 Created é o status que nossa API retorna
        setStatusMessage('Obrigado pelo seu feedback!');
        // Limpa o formulário após o envio
        setScore(null);
        setComment('');
      }
    } catch (error) {
      // Lidard com erros
      console.error("Ocorreu um erro ao enviar o feedback:", error);
      setStatusMessage('Falha ao enviar o feedback. Tente novamente mais tarde.');
    }
  };

  return (
    <div className="feedback-form-container">
      <h2>Deixe seu Feedback</h2>
      <p>Em uma escala de 0 a 10, o quão provável você é de nos recomendar?</p>
      
      <div className="score-buttons">
        {[...Array(11).keys()].map((number) => (
          <button
            key={number}
            type="button"
            className={`score-btn ${score === number ? 'selected' : ''}`}
            onClick={() => setScore(number)}
          >
            {number}
          </button>
        ))}
      </div>

      <div className="comment-section">
        <label htmlFor="comment">Deixe um comentário (opcional):</label>
        <textarea
          id="comment"
          rows="4"
          value={comment}
          onChange={(e) => setComment(e.target.value)}
        />
      </div>

      <button type="submit" onClick={handleSubmit} className="submit-btn">
        Enviar Feedback
      </button>

      {/* Exibe a mensagem de status para o usuário */}
      {statusMessage && <p className="status-message">{statusMessage}</p>}
    </div>
  );
}

export default FeedbackForm;