import './App.css'
import 'bootstrap/dist/css/bootstrap.min.css';
import CandidatesPage from './components/candidatesPage'
import {
  QueryClient,
  QueryClientProvider,
} from '@tanstack/react-query'

function App() {
  const queryClient = new QueryClient();
  return (
    <>
      <script type="module" src="https://cdn.skypack.dev/twind/shim"></script>
      <QueryClientProvider client={queryClient}>
        <CandidatesPage />
      </QueryClientProvider>
    </>
  )
}

export default App
