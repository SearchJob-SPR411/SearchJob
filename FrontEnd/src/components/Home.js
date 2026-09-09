import { useState } from "react";
import "./Home.css";

const VACANCIES = [
  {
    id: 1,
    title: "Frontend Developer",
    company: "Nova Tech",
    location: "Kyiv, Ukraine",
    type: "Full-time",
    salary: "$1,800 – $2,500",
    posted: "2 days ago",
  },
  {
    id: 2,
    title: "Product Designer",
    company: "Studio Loop",
    location: "Remote",
    type: "Full-time",
    salary: "$1,500 – $2,200",
    posted: "3 days ago",
  },
  {
    id: 3,
    title: "Customer Support Specialist",
    company: "Helpdesk Co.",
    location: "Lviv, Ukraine",
    type: "Part-time",
    salary: "$700 – $900",
    posted: "5 days ago",
  },
  {
    id: 4,
    title: "Backend Engineer (Node.js)",
    company: "Rivergate",
    location: "Remote",
    type: "Full-time",
    salary: "$2,000 – $3,000",
    posted: "1 week ago",
  },
  {
    id: 5,
    title: "Marketing Manager",
    company: "Brightside",
    location: "Kyiv, Ukraine",
    type: "Full-time",
    salary: "$1,300 – $1,900",
    posted: "1 week ago",
  },
  {
    id: 6,
    title: "Warehouse Assistant",
    company: "QuickShip",
    location: "Odesa, Ukraine",
    type: "Full-time",
    salary: "$600 – $750",
    posted: "2 weeks ago",
  },
];

export default function Home() {
  const [query, setQuery] = useState("");

  const filtered = VACANCIES.filter((v) =>
    (v.title + v.company + v.location)
      .toLowerCase()
      .includes(query.toLowerCase())
  );

  return (
    <div className="page">
      <header className="header">
        <div className="header-inner">
          <div className="logo">
            Search<span className="logo-accent">Job</span>
          </div>
          <nav className="nav">
            <a href="#!">Vacancies</a>
            <a href="#!">Companies</a>
            <a href="#!">About</a>
          </nav>
          <button className="btn btn-primary">Post a job</button>
        </div>
      </header>

      
      <section className="hero">
        <div className="hero-inner">
          <h1>Find your next job</h1>
          <p>Browse open vacancies from companies hiring right now.</p>

          <div className="search-box">
            <span className="search-icon">🔍</span>
            <input
              type="text"
              value={query}
              onChange={(e) => setQuery(e.target.value)}
              placeholder="Search by title, company or location"
            />
          </div>
        </div>
      </section>

      
      <main className="main">
        <div className="main-header">
          <h2>Open vacancies</h2>
          <span className="results-count">{filtered.length} results</span>
        </div>

        <div className="vacancy-list">
          {filtered.map((job) => (
            <div className="vacancy-card" key={job.id}>
              <div className="vacancy-info">
                <h3>{job.title}</h3>
                <p className="company">{job.company}</p>
                <div className="meta">
                  <span>📍 {job.location}</span>
                  <span>💼 {job.type}</span>
                  <span>🕒 {job.posted}</span>
                </div>
              </div>

              <div className="vacancy-action">
                <span className="salary">{job.salary}</span>
                <button className="btn btn-primary">Apply now</button>
              </div>
            </div>
          ))}

          {filtered.length === 0 && (
            <p className="no-results">No vacancies match "{query}".</p>
          )}
        </div>
      </main>

      
      <footer className="footer">
        <div className="footer-inner">© 2026 SearchJob. All rights reserved.</div>
      </footer>
    </div>
  );
}