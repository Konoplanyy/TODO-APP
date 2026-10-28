using Application.DTOs;
using AutoMapper;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Mapper
{
    public class Mapper: Profile
    {
        public Mapper()
        {
            CreateMap<Category, CategoryDto>();
            CreateMap<User, UserDto>();
            CreateMap<TaskItem, TaskItemDto>();
            CreateMap<TaskItem, TaskItemDto>()
                .ConstructUsing(item => new TaskItemDto(item.Id, item.Name, item.Description, item.Due_date, item.IsCompleted, item.CategoryId, item.CreatedAt, item.LastUpdatedAt));

        }
    }
}
