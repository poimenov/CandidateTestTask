import axios from 'axios';
import React, { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { CandidateModel } from '../core/models/candidate.model';
import CandidateForm from '../components/candidateForm';

const EditCandidate: React.FC = () => {
  const { email } = useParams<{ email: string }>();
  const navigate = useNavigate();
  const [candidate, setCandidate] = useState<CandidateModel | null>(null);

  useEffect(() => {
    axios.get(`/candidate/${email}`)
      .then(response => setCandidate(response.data))
      .catch(error => console.error(error));
  }, [email]);

  const handleSubmit = () => {
    navigate('/');
  };

  return (
    <div>
      <h1>Edit Candidate</h1>
      {candidate && <CandidateForm candidate={candidate} onSubmit={handleSubmit} />}
    </div>
  );
};
export default EditCandidate;
