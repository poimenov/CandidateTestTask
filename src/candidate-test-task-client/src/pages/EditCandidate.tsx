import React, { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { CandidateModel } from '../core/models/candidate.model';
import CandidateForm from '../components/candidateForm';
import { getOne } from '../core/service';

const EditCandidate: React.FC = () => {
  const { email } = useParams<{ email: string }>();
  const navigate = useNavigate();
  const [candidate, setCandidate] = useState<CandidateModel | null>(null);

  useEffect(() => {
    if (email) {
      getOne(email).then(setCandidate).catch(error => console.error(error));
    }
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
