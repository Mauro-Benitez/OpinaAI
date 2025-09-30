import React, { useState } from 'react'; // 1. Importe o useState
import './FeedbackForm.css'; // Vamos criar este arquivo para um estilo básico

function FeedbackForm() {
  // 2. Crie os estados para guardar os dados do formulário
  const [score, setScore] = useState(null); // 'null' significa que nenhuma nota foi selecionada ainda
  const [comment, setComment] = useState('');

  const handleSubmit = (event) => {
    event.preventDefault(); // Impede o recarregamento padrão da página ao submeter
    // Lógica de envio para a API virá aqui
    alert(`Nota: ${score}\nComentário: ${comment}`);
  };

  return (
    <div className="feedback-form-container">
      <h2>Deixe seu Feedback</h2>
      <p>Em uma escala de 0 a 10, o quão provável você é de nos recomendar?</p>
      
      {/* Container para os botões de nota */}
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

      {/* Área de texto para o comentário */}
      <div className="comment-section">
        <label htmlFor="comment">Deixe um comentário (opcional):</label>
        <textarea
          id="comment"
          rows="4"
          value={comment}
          onChange={(e) => setComment(e.target.value)}
        />
      </div>

      {/* Botão de envio */}
      <button type="submit" onClick={handleSubmit} className="submit-btn">
        Enviar Feedback
      </button>
    </div>
  );
}

export default FeedbackForm;