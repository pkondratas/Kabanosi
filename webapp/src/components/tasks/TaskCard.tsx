"use client";

import { useDispatch } from 'react-redux';
import { toggleTask } from '../../store/slices/tasksSlice';

interface TaskCardProps {
  id: number;
  title: string;
  completed: boolean;
}

export default function TaskCard({ id, title, completed }: TaskCardProps) {
  const dispatch = useDispatch();

  return (
    <div>
      <h3>{title}</h3>
      <input
        type="checkbox"
        checked={completed}
        onChange={() => dispatch(toggleTask(id))}
      />
    </div>
  );
}