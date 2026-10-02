import { BrowserRouter, Routes, Route } from "react-router-dom";
import DefaultLayout from "./components/layouts/DefaultLayout";
import Navbar from "./components/navbar/Navbar";
import Home from "./pages/home/Home";
import VacancyDetails from "./pages/vacancy/VacancyDetails";
import Companies from "./pages/companies/Companies";
import CompanyDetails from "./pages/companies/CompanyDetails";

function App() {
  return (
    <BrowserRouter>
      <DefaultLayout>
        <Navbar />

        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/vacancies/:id" element={<VacancyDetails />} />
          <Route path="/companies" element={<Companies />} />
          <Route path="/companies/:id" element={<CompanyDetails />} />
        </Routes>
      </DefaultLayout>
    </BrowserRouter>
  );
}

export default App;