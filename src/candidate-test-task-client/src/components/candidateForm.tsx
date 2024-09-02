import React, { useState } from 'react';
import { CandidateModel } from '../core/models/candidate.model';
import queryClient from '../queryClient';
import { useMutation } from '@tanstack/react-query';
import { postOne } from '../core/service';

interface CandidateFormProps {
  candidate?: CandidateModel;
  onSubmit: () => void;
}

const CandidateForm: React.FC<CandidateFormProps> = ({ candidate, onSubmit }) => {
  const [formData, setFormData] = useState({
    email: candidate?.email || '',
    firstName: candidate?.firstName || '',
    lastName: candidate?.lastName || '',
    phoneNumber: candidate?.phoneNumber || '',
    linkedInUrl: candidate?.linkedInUrl || '',
    gitHubUrl: candidate?.gitHubUrl || '',
    startTime: candidate?.timeInterval?.startTime || '',
    endTime: candidate?.timeInterval?.endTime || '',
  });

  const mutation = useMutation(
    {
        mutationFn: (newCandidate: CandidateModel) => postOne(newCandidate),
        onSuccess: () => {
          queryClient.invalidateQueries({ queryKey: ['candidates'] })
          onSubmit()
        },
    }
  );

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    const newCandidate: CandidateModel = {
      ...formData,
      timeInterval: {
        startTime: formData.startTime,
        endTime: formData.endTime,
      },
    };
    mutation.mutate(newCandidate);
  };

  return (
    <form onSubmit={handleSubmit}>
      <div className='form-group'>
        <label htmlFor="email">Email</label>
        <input type="email" name="email" id="email" className="form-control" value={formData.email} onChange={handleChange} />
      </div>
      <div className='form-group'>
        <label htmlFor='firstName'>First Name</label>
        <input type="text" name="firstName" id="firstName" className="form-control" value={formData.firstName} onChange={handleChange} />
      </div>
      <div className='form-group'>
        <label htmlFor='lastName'>Last Name</label>
        <input type="text" name="lastName" id="lastName" className="form-control" value={formData.lastName} onChange={handleChange} />
      </div>
      <div className='form-group'>
        <label htmlFor='phoneNumber'>Phone Number</label>
        <input type="tel" name="phoneNumber" id="phoneNumber" className="form-control" value={formData.phoneNumber} onChange={handleChange} />
      </div>
      <div className='form-group'>
        <label htmlFor='linkedInUrl'>LinkedIn URL</label>
        <input type="url" name="linkedInUrl" id="linkedInUrl" className="form-control" value={formData.linkedInUrl} onChange={handleChange} />
      </div>
      <div>
        <label htmlFor='gitHubUrl'>GitHub URL</label>
        <input type="url" name="gitHubUrl" id="gitHubUrl" className="form-control" value={formData.gitHubUrl} onChange={handleChange} />
      </div>
      <div className='form-group'>
        <label htmlFor='startTime'>Start Time</label>
        <input type="time" name="startTime" id="startTime" className="form-control" value={formData.startTime} onChange={handleChange} />
      </div>
      <div className='form-group'>
        <label htmlFor='endTime'>End Time</label>
        <input type="time" name="endTime" id="endTime" className="form-control" value={formData.endTime} onChange={handleChange} />
      </div>      
      <button className="btn btn-primary" type="submit">Submit</button>
    </form>
  );
};

export default CandidateForm;
