import React from 'react';
import { useNavigate } from 'react-router-dom';
import CandidateForm from '../components/candidateForm';

const AddCandidate: React.FC = () => {
  const navigate = useNavigate();

  const handleSubmit = () => {
    navigate('/');
  };

  return (
    <div>
      <h1>Add Candidate</h1>
      <CandidateForm onSubmit={handleSubmit} />
    </div>
  );
};

export default AddCandidate;
