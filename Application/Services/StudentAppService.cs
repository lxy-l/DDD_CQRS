﻿using System;
using System.Collections.Generic;

using Application.Interfaces;
using Application.ViewModel;

using AutoMapper;
using AutoMapper.QueryableExtensions;

using Domain.Interfaces;
using Domain.Models;

namespace Application.Services
{
    /// <summary>
    /// StudentAppService 服务接口实现类,继承 服务接口
    /// 通过 DTO 实现视图模型和领域模型的关系处理
    /// 作为调度者，协调领域层和基础层，
    /// 这里只是做一个面向用户用例的服务接口,不包含业务规则或者知识
    /// </summary>
    public class StudentAppService : IStudentAppService
    {
        // 注意这里是要IoC依赖注入的，还没有实现
        private readonly IStudentRepository _StudentRepository;
        // 用来进行DTO
        private readonly IMapper _mapper;
        public StudentAppService(IStudentRepository StudentRepository,IMapper mapper)
        {
            _StudentRepository = StudentRepository;
            _mapper = mapper;
        }

        public IEnumerable<StudentViewModel> GetAll()
        {

            //第一种写法 Map
            return _mapper.Map<IEnumerable<StudentViewModel>>(_StudentRepository.GetAll());

            //第二种写法 ProjectTo
            //return (_StudentRepository.GetAll()).ProjectTo<StudentViewModel>(_mapper.ConfigurationProvider);
        }

        public StudentViewModel GetById(Guid id)
        {
            return _mapper.Map<StudentViewModel>(_StudentRepository.GetById(id));
        }

        public void Register(StudentViewModel StudentViewModel)
        {
            //这里引入领域设计中的写命令 还没有实现
            //请注意这里如果是平时的写法，必须要引入Student领域模型，会造成污染

            _StudentRepository.Add(_mapper.Map<Student>(StudentViewModel));
            _StudentRepository.SaveChanges();
        }

        public void Update(StudentViewModel StudentViewModel)
        {
            _StudentRepository.Update(_mapper.Map<Student>(StudentViewModel));
            _StudentRepository.SaveChanges();
        }

        public void Remove(Guid id)
        {
            _StudentRepository.Remove(id);
            _StudentRepository.SaveChanges();

        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
