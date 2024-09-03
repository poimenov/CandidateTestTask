import React, { useState } from 'react';
import { CandidateModel } from '../core/models/candidate.model';
import queryClient from '../queryClient';
import { useMutation } from '@tanstack/react-query';
import { postOne } from '../core/service';
import Button from 'react-bootstrap/Button';
import Form from 'react-bootstrap/Form';
import Row from 'react-bootstrap/Row';

interface CandidateFormProps {
  candidate?: CandidateModel;
  onSubmit: () => void;
}

const CandidateForm: React.FC<CandidateFormProps> = ({ candidate, onSubmit }) => {
  const originalCandidate = candidate;

  const [formData, setFormData] = useState({
    email: candidate?.email || '',
    firstName: candidate?.firstName || '',
    lastName: candidate?.lastName || '',
    phoneNumber: candidate?.phoneNumber || '',
    linkedInUrl: candidate?.linkedInUrl || '',
    gitHubUrl: candidate?.gitHubUrl || '',
    comment: candidate?.comment || '',
    startTime: getTime(candidate?.timeInterval?.startTime || ''),
    endTime: getTime(candidate?.timeInterval?.endTime || ''),
  });

  const [validated, setValidated] = useState(false);

  const [enabled, setEnabled] = useState(false);

  const mutation = useMutation(
    {
        mutationFn: (newCandidate: CandidateModel) => postOne(newCandidate),
        onSuccess: () => {
          queryClient.invalidateQueries({ queryKey: ['candidates'] })
          onSubmit()
        },
    }
  );

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
    
    switch (e.target.name) {
      case 'email':
        setEnabled(e.target.value !== originalCandidate?.email);
        break;
      case 'firstName':
        setEnabled(e.target.value !== originalCandidate?.firstName);
        break;
      case 'lastName':
        setEnabled(e.target.value !== originalCandidate?.lastName);
        break;
      case 'phoneNumber':
        setEnabled(e.target.value !== originalCandidate?.phoneNumber);
        break;
      case 'linkedInUrl':
        setEnabled(e.target.value !== originalCandidate?.linkedInUrl);
        break;
      case 'gitHubUrl':
        setEnabled(e.target.value !== originalCandidate?.gitHubUrl);
        break;
      case 'comment':
        setEnabled(e.target.value !== originalCandidate?.comment);
        break;
      case 'startTime':
        setEnabled(e.target.value !== originalCandidate?.timeInterval?.startTime);
        break;
      case 'endTime':
        setEnabled(e.target.value !== originalCandidate?.timeInterval?.endTime);
        break;
    }
  };

  const handleReset = () => {
    setFormData({
      email: originalCandidate?.email || '',
      firstName: originalCandidate?.firstName || '',
      lastName: originalCandidate?.lastName || '',
      phoneNumber: originalCandidate?.phoneNumber || '',
      linkedInUrl: originalCandidate?.linkedInUrl || '',
      gitHubUrl: originalCandidate?.gitHubUrl || '',
      comment: originalCandidate?.comment || '',
      startTime: getTime(originalCandidate?.timeInterval?.startTime || ''),
      endTime: getTime(originalCandidate?.timeInterval?.endTime || ''),
    });
  };

  function getTime(time: string) {
    if (time === '') {
      return '00:00';
    }

    const arrTime = time.split(':');
    return `${arrTime[0]}:${arrTime[1]}`;
  }

  const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
    const form = e.currentTarget;
    e.preventDefault();
    if (form.checkValidity() === false) {
      e.stopPropagation();
      setValidated(false);
    }

    setValidated(true);    
    const newCandidate: CandidateModel = {
      ...formData,
      timeInterval: {
        startTime: formData.startTime + ":00",
        endTime: formData.endTime + ":00",
      },
    };
    mutation.mutate(newCandidate);
  };

  return (
    <Form validated={validated} onSubmit={handleSubmit}>
      <Form.Group className="mb-3">
        <Form.Label htmlFor="email">Email</Form.Label>
        <Form.Control type="email" name="email" id="email" maxLength={150} className="form-control" required value={formData.email} onChange={handleChange} />    
      </Form.Group>
      <Form.Group className="mb-3">
        <Form.Label htmlFor='firstName'>First Name</Form.Label>
        <Form.Control type="text" name="firstName" id="firstName" maxLength={50} className="form-control" required value={formData.firstName} onChange={handleChange} />
      </Form.Group>
      <Form.Group className="mb-3">
        <Form.Label htmlFor='lastName'>Last Name</Form.Label>
        <Form.Control type="text" name="lastName" id="lastName" maxLength={50} className="form-control" required value={formData.lastName} onChange={handleChange} />
      </Form.Group>
      <Form.Group className="mb-3">
        <Form.Label htmlFor='phoneNumber'>Phone Number</Form.Label>
        <Form.Control type="tel" name="phoneNumber" id="phoneNumber" maxLength={25} className="form-control" value={formData.phoneNumber} onChange={handleChange} />
      </Form.Group>
      <Form.Group className="mb-3">
        <Form.Label htmlFor='linkedInUrl'>LinkedIn URL</Form.Label>
        <Form.Control type="url" name="linkedInUrl" id="linkedInUrl" maxLength={250} className="form-control" value={formData.linkedInUrl} onChange={handleChange} />
      </Form.Group>
      <Form.Group className="mb-3">
        <Form.Label htmlFor='gitHubUrl'>GitHub URL</Form.Label>
        <Form.Control type="url" name="gitHubUrl" id="gitHubUrl" maxLength={250} className="form-control" value={formData.gitHubUrl} onChange={handleChange} />
      </Form.Group>
      <Form.Group className="mb-3">
        <Form.Label htmlFor='startTime'>Start Time</Form.Label>
        <Form.Control type="time" name="startTime" id="startTime" className="form-control" min="08:00" max="14:00" step="900" required value={formData.startTime} onChange={handleChange} />
      </Form.Group>
      <Form.Group className="mb-3">
        <Form.Label htmlFor='endTime'>End Time</Form.Label>
        <Form.Control type="time" name="endTime" id="endTime" className="form-control" min="15:00" max="19:00" step="900" required value={formData.endTime} onChange={handleChange} />
      </Form.Group> 
      <Form.Group className="mb-3">
        <Form.Label htmlFor='comment'>Comment</Form.Label>
        <Form.Control as="textarea" rows={3} name="comment" id="comment" className="form-control" required value={formData.comment} onChange={handleChange} />
      </Form.Group>
      <Row xs="4">
        <Button className='m-1' type="reset" onClick={handleReset}>Reset</Button>
        <Button className='m-1' variant='primary' type="submit" disabled={!enabled}>Submit</Button>      
      </Row>     
    </Form>
  );
};
export default CandidateForm;
