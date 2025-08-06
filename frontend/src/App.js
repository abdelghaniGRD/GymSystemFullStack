import "./App.css";
import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
import Login from "./Login";
import Register from "./Register";
import Dashboard from "./Dashboard";
import Members from "./Members";
import Plans from "./Plans";
import Summary from "./Summary";

function App() {
  return (
    <Router>
      <div className="App">
        <Routes>
          <Route path="/account/login" element={<Login />}></Route>
          <Route path="/account/register" element={<Register />}></Route>
          <Route path="/dashboard" element={<Dashboard />}>
            <Route index element={<Summary />} />
            <Route path="members" element={<Members />} />
            <Route path="plans" element={<Plans />} />
          </Route>
        </Routes>
      </div>
    </Router>
  );
}

export default App;
