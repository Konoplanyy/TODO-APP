export type Category = {
  id: string;
  name: string;
  color: string;
  createdAt: string;
  lastUpdatedAt: string;
};

export type CategoryInput = Pick<Category, 'name' | 'color'>;
