import { BrowserRouter, Routes, Route } from "react-router-dom";
import DefaultLayout from "./components/layouts/DefaultLayout";
import Navbar from "./components/navbar/Navbar";
import Home from "./pages/home/Home";
import VacancyDetails from "./pages/vacancy/VacancyDetails";
import CreateVacancy from "./pages/vacancy/CreateVacancy";
import Companies from "./pages/companies/Companies";
import CompanyDetails from "./pages/companies/CompanyDetails";
import Favorites from "./pages/favorites/Favorites";
import MyApplications from "./pages/applications/MyApplications";
import VacancyApplications from "./pages/applications/VacancyApplications";
import EmployerDashboard from "./pages/employer/EmployerDashboard";

function App() {
  return (
    <BrowserRouter>
      <DefaultLayout>
        <Navbar />

        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/vacancies/create" element={<CreateVacancy />} />
          <Route path="/vacancies/:id" element={<VacancyDetails />} />
          <Route path="/vacancies/:id/applications" element={<VacancyApplications />} />
          <Route path="/favorites" element={<Favorites />} />
          <Route path="/applications" element={<MyApplications />} />
          <Route path="/companies" element={<Companies />} />
          <Route path="/companies/:id" element={<CompanyDetails />} />
          <Route path="/employer/dashboard" element={<EmployerDashboard />} />
        </Routes>
      </DefaultLayout>
    </BrowserRouter>
  );
}

export default App;