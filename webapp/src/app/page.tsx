"use client";

import { useSelector } from 'react-redux';
import TaskForm from '../components/tasks/TaskForm';
import TaskCard from '../components/tasks/TaskCard';
import { RootState } from '../store/store';

export default function Home() {
  const tasks = useSelector((state: RootState) => state.tasks.tasks);

  return (
    <div style={{ padding: '20px' }}>
      <h1>Kabanosi Task Management System</h1>
      <TaskForm />
      <div>
        {tasks.map((task) => (
          <TaskCard
            key={task.id}
            id={task.id}
            title={task.title}
            completed={task.completed}
          />
        ))}
      </div>
    </div>
  );
}