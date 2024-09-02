import React from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import { QueryClientProvider } from '@tanstack/react-query';
import queryClient from './queryClient';
import Home from './pages/Home';
import AddCandidate from './pages/AddCandidate';
import EditCandidate from './pages/EditCandidate';

const App: React.FC = () => {
  return (
    <QueryClientProvider client={queryClient}>
      <Router>
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/add" element={<AddCandidate />} />
          <Route path="/edit/:email" element={<EditCandidate />} />
        </Routes>
      </Router>
    </QueryClientProvider>
  );
};

export default App;

